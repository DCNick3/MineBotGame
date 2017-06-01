using log4net.Appender;
using log4net.Core;
using System;

namespace MineBotGame
{
    /// <summary>
    /// Log things, does not matter
    /// </summary>
    public class ControlLogAppender : AppenderSkeleton
    {
        public ControlLogAppender()
        {
            //workerThread = new Thread(Worker);
            //workerThread.Start();
            //form.loadEvent.WaitOne();
        }

        //Thread workerThread;
        //ControlForm form = new ControlForm();
        /*private void Worker()
        {
            Application.EnableVisualStyles();
            Application.Run(form);
        }*/


        protected override void Append(LoggingEvent loggingEvent)
        {
            ControlForm form = Program.controlForm;
            if (form != null && form.isLoaded)
            {
                string v = RenderLoggingEvent(loggingEvent);
                if (!form.isFormClosing && form.formCloseMutex.WaitOne(1))
                {
                    form.Invoke(new Action(() =>
                    {
                        form.Append(v);
                    }
                    ));
                    form.formCloseMutex.ReleaseMutex();
                }
            }
        }
    }
}
