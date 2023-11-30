from _base import *

#-------------------------------------------------------------------------------

def clean():

  print('\nCI: CLEAN\n')
  chdir(sln_dir)

  result = run(shell=True, capture_output=False, args=[
    options['msbuild_exe'],
    options['solution'],
    '-target:clean',
    f'-maxCpuCount:{options["maximum_cpus"]}',
    '-verbosity:quiet',
    '-detailedSummary:false'
  ])

  file_patterns = [ 
    '*.csproj.user', 
    '*.vbproj.user' 
    '*.vcxproj.user',
    ]
  
  dir_patterns = [ 
    '__pycache__',
    '.vs', 
    'bin',
    'dist', 
    'obj', 
    'TestResults',
    "packages",
    "node_modules"
    ]
  
  for p in [f for p in file_patterns for f in sln_dir.rglob(p)]:
    p.unlink(missing_ok=True)
    print(f'Deleted: {p}')

  for p in [f for p in dir_patterns for f in sln_dir.rglob(p)]:
    if p.exists():
      rmtree(str(p), ignore_errors=False, onerror=print)
      print(f'Deleted: {p}')

  return True # TODO: parse output(s) to determine success

#-------------------------------------------------------------------------------

if __name__ == '__main__':
  clean()