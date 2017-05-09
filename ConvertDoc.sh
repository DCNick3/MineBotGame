#!/bin/bash

function mdtohtml {
	echo "==> $1 -> $2"
	rm -f $2
	pandoc $1 -o $2
	sed -i '1s/^/<meta charset="UTF-8"> <style> table { width: 100% !important; } table, th, td { border: 1px solid black; } .caption { font-style: italic; } <\/style>\n/' $2
}

set -e


mdtohtml MineBotGameSpec.md  MineBotGameSpec.html
mdtohtml MineBotGameProtocol.md  MineBotGameProtocol.html
mdtohtml MineBotGameRealization.md  MineBotGameRealization.html