from _base import *

#-------------------------------------------------------------------------------

def pack():
  
  print('\nCI: PACK\n')
  chdir(sln_dir)

  result = run(shell=True, capture_output=False, args=[
    options['msbuild_exe'],
    options['sdk_project'],
    '-restore',
    '-target:pack',
    f'-maxCpuCount:{options["maximum_cpus"]}',
    '-verbosity:normal',
    '-detailedSummary:true',
    f'/p:Configuration={options["configuration"]}',
    f'/p:Platform={options["platform"]}',
  ])
  
  return True # TODO: parse output(s) to determine success

#-------------------------------------------------------------------------------

if __name__ == '__main__':
  pack()