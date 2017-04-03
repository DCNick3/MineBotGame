using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Execution.VM;
using System.Numerics;
using System.Threading;

namespace MineBotGame
{
    public class LuaController : PlayerController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
       (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string[] errors = new string[]
            {
                "No error",//0
            };
        private static string botBootstrap = @"

local raw_yield = __bot_yield;
__bot_yield = nil;

local function bot_yi(...)
    return table.unpack(raw_yield({...}));
end
--Define functions~~

function idle()
    return bot_yi(0);
end

function move(a,b)
    local x, y
    if type(a) == 'table' then
        x = a[1];
        y = b[2];
    elseif type(a) == 'number' and type(b) == 'number' then
        x = a;
        y = b;
    else
        return false, 'Invalid parameters'
    end
    return bot_yi(1,x,y);
end

function discover()
    return bot_yi(4);
end


local function string(o)
    return '\'' .. tostring(o) .. '\''
end

local function recurse(o, indent)
    if indent == nil then indent = '' end
    local indent2 = indent.. '  '
    if type(o) == 'table' then
        local s = indent.. '{' .. '\n'
        local first = true
        for k,v in pairs(o) do
            if first == false then s = s.. ', \n' end
            if type(k) ~= 'number' then k = string(k) end
            s = s..indent2.. '[' .. k.. '] = ' .. recurse(v, indent2)
            first = false
        end
        return s.. '\n' .. indent.. '}'
    else
        return string(o)
    end
end

function var_dump(...)
    local args = {...}
    if #args > 1 then
        var_dump(args)
    else
        print(recurse(args[1]))
    end
end

function new_matrix(x0,y0,x1,y1,v)
    local r = {}
    for i = x0, x1 do
        r[i] = {}
        for j = y0, y1 do
            r[i][j] = b;
        end
    end
    return r;
end

";

        private Table LuaMatrix(Script owner, int[,] src, int offsetX, int offsetY)
        {
            Table t = new Table(owner);
            for (int i = 0; i < src.GetLength(0); i++)
            {
                t.Set(DynValue.NewNumber(offsetX + i + 1), DynValue.NewTable(new Table(owner)));
                for (int j = 0; j < src.GetLength(1); j++)
                {
                    t.Get(DynValue.NewNumber(offsetX + i + 1)).Table.Set(DynValue.NewNumber(offsetY + j + 1), DynValue.NewNumber(src[i, j]));
                }
            }
            return t;
        }

        private Table LuaYielder(Table data)
        {
            var s = data.Pairs.Where((_) => _.Key.Type == DataType.Number).OrderBy((_) => _.Key.Number).GetEnumerator();
            if (!s.MoveNext())
                return new Table(data.OwnerScript, DynValue.NewBoolean(false), DynValue.NewString("Empty table was given"));
            PlayerAction.Type type = (PlayerAction.Type)(int)s.Current.Value.Number;
            switch (type)
            {
                case PlayerAction.Type.Idle:
                    botToProgram = new PlayerAction();
                    botToProgram.type = type;
                    botToProgramEvent.Set();
                    break;

                default: /* Whoops */
                    botToProgram = new PlayerAction();
                    botToProgram.type = PlayerAction.Type.Idle;
                    botToProgramEvent.Set();
                    /* TODO: Log it!! */
                    break;
            }
            if (WaitHandle.WaitAny(new WaitHandle[] { programToBotEvent/*, cancelToken.WaitHandle*/}) == 1)
            {
                log.Debug("Bot received cancel signal. Exitting.");
                throw new OperationCanceledException();
            }
            programToBotEvent.Reset();

            Table res = new Table(data.OwnerScript);

            res.Set(1, DynValue.NewBoolean(programToBot.IsSucceed));
            
            return res;
        }

        protected override void BotWorker()
        {
            Script sc = new Script();
            
            sc.Options.DebugPrint = s => log.DebugFormat("[BOT-OUT] {0}", s);

            //log.Debug("Staring bootstrap script");
            sc.Globals["__bot_yield"] = (Func<Table, Table>)LuaYielder;
            sc.DoString(botBootstrap, null, "INTERNAL_BOT_BOOTSTRAP");

            //log.Debug("Entering loop");
            while (true)
            {
                try
                {
                    sc.DoString(@"
local map = new_matrix(-64,-64,64,64,-1);
local pos = {0,0}
while true do
    idle();
end
", null, "BOT_CODE");
                    /*
                     
--[[
local di = 1;
while true do
    local dir = dirs[di];

    --var_dump(di, dir);
    local _,r = discover();
    --var_dump(r);
    while r[dir[1]][dir[2]] == 0 do
        move(dir[1], dir[2]);
        _,r = discover();
    end
    

    di = di + 1;
    if di > #dirs then
        di = 1
    end
end]

                    local funtion enum_neigh(dirs)
    local i = -1
    local j = -1
    return function()
        local r = {i,j}
        repeat
            i = i + 1;
            if i == 2 then
                j = j + 1;
                i = 0;
            end
        until i ~= 0 or j ~= 0;
    end
end




local dirs = {{0,1},{1,0},{0,-1},{-1,0}};
local ds = 1;
local d = dirs[ds];
local function nd()
    ds = ds + 1;
    if ds > #dirs then
        ds = 1
    end
    d = dirs[ds];
    idle();
    --var_dump(d);
end
local function pd()
    ds = ds - 1;
    if ds < 1 then
        ds = #dirs;
    end
    d = dirs[ds];
    idle();
    --var_dump(d);
end

local _,r = discover()
while (r[d[1]][d[2]] == 0) do
    nd();
end
nd();

while true do
    local _,r = discover()
    --var_dump(d);
    pd()
    local ds0 = ds;
    local ht = 0
    while (r[d[1]][d[2]] ~= 0) do
        nd();
    end
    --var_dump(r);
    --print(""moving to ""..tostring(ds))
    move(d[1],d[2]);
end
                     */
                }
                catch (ScriptRuntimeException ex)
                {
                    log.Warn("Doh! An error occured while running bot #" + Id +"; It'll be restarted\r\n" + ex.DecoratedMessage, ex);
                }
                catch (SyntaxErrorException ex)
                {
                    log.Error("Doh! Syntax error in bot#" + Id + " code; It'll be killed\r\n" + ex.DecoratedMessage, ex);
                    return;
                }
            }
        }
    }
}
