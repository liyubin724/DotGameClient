ECHO OFF

SET LibDir=..\DotGameScripts\Dot
SET EditorTargetDir=Assets\DotEngine\Editor
SET RuntimeTargetDir=Assets\DotEngine


COPY %LibDir%\libs\log4net.dll %RuntimeTargetDir%
COPY %LibDir%\libs\Newtonsoft.Json.dll %RuntimeTargetDir%
COPY %LibDir%\libs\Newtonsoft.Json.dll %RuntimeTargetDir%

COPY %LibDir%\libs\Editor\ReflectionMagic.dll %EditorTargetDir%

REM ---------------------------------------------------------------------------

COPY %LibDir%\DotEngine\bin\Debug\DotEngine.dll %RuntimeTargetDir%
COPY %LibDir%\DotEngine\bin\Debug\DotEngine.pdb %RuntimeTargetDir%

COPY %LibDir%\Context\DotEngine.Context\bin\Debug\DotEngine.Context.dll %RuntimeTargetDir%
COPY %LibDir%\Context\DotEngine.Context\bin\Debug\DotEngine.Context.pdb %RuntimeTargetDir%

COPY %LibDir%\Framework\DotEngine.Framework\bin\Debug\DotEngine.Framework.dll %RuntimeTargetDir%
COPY %LibDir%\Framework\DotEngine.Framework\bin\Debug\DotEngine.Framework.pdb %RuntimeTargetDir%

COPY %LibDir%\Log\DotEngine.Log\bin\Debug\DotEngine.Log.dll %RuntimeTargetDir%
COPY %LibDir%\Log\DotEngine.Log\bin\Debug\DotEngine.Log.pdb %RuntimeTargetDir%

COPY %LibDir%\Others\PriorityQueue\bin\Debug\PriorityQueue.dll %RuntimeTargetDir%
COPY %LibDir%\Others\PriorityQueue\bin\Debug\PriorityQueue.pdb %RuntimeTargetDir%

REM -----------------------------------------------------------------------

COPY %LibDir%\DotEditor\bin\Debug\DotEditor.dll %EditorTargetDir%
COPY %LibDir%\DotEditor\bin\Debug\DotEditor.pdb %EditorTargetDir%

COPY %LibDir%\Tools\DotTool.ScriptGenerate\bin\Debug\DotTool.ScriptGenerate.dll %EditorTargetDir%
COPY %LibDir%\Tools\DotTool.ScriptGenerate\bin\Debug\DotTool.ScriptGenerate.pdb %EditorTargetDir%

PAUSE