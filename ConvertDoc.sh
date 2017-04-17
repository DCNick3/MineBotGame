#!/bin/bash
pandoc MineBotGameSpec.md -o MineBotGameSpec.html
sed -i '1s/^/<meta charset="UTF-8"> <style> table, th, td { border: 1px solid black; } .caption { font-style: italic; } <\/style>\n/' MineBotGameSpec.html
pandoc MineBotGameProtocol.md -o MineBotGameProtocol.html
sed -i '1s/^/<meta charset="UTF-8"> <style> table, th, td { border: 1px solid black; } .caption { font-style: italic; } <\/style>\n/' MineBotGameProtocol.html