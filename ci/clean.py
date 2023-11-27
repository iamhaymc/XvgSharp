from _util import *

chdir(sln_dir)

run(["dotnet", "clean"])

for p in test_dir.rglob("out"):
  rmtree(p, ignore_errors=False)