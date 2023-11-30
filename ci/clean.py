from _util import *

DELETE_FILE_PATTERNS = []
DELETE_DIR_PATTERNS = ['out']

chdir(sln_dir)

run(["dotnet", "clean"])

for p in [f for p in DELETE_FILE_PATTERNS for f in sln_dir.rglob(p)]:
  p.unlink(missing_ok=True)
  print(f'Deleted: {p}')

for p in [f for p in DELETE_DIR_PATTERNS for f in sln_dir.rglob(p)]:
  if p.exists():
    rmtree(str(p), ignore_errors=False, onerror=print)
    print(f'Deleted: {p}')