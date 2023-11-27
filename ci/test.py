from _util import *

chdir(sln_dir)

proj_dir = sln_dir.joinpath('test/Tests')
proj_file = proj_dir.joinpath("Tests.csproj")
proj_out_dir = proj_dir.joinpath("out")

run(["dotnet", "test", str(proj_file),
  "--results-directory", str(proj_out_dir),
  f'--logger:"xunit;LogFilePath={proj_out_dir.joinpath('result.xml')}',
  '--collect:"XPlat Code Coverage"' 
])