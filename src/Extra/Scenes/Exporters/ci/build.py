from _base import *

#-------------------------------------------------------------------------------

def build():
  
  print('\nCI: BUILD\n')
  chdir(sln_dir)

  result = run(shell=True, capture_output=False, args=[
    options['msbuild_exe'],
    options['solution'],
    '-restore',
    f'-maxCpuCount:{options["maximum_cpus"]}',
    '-verbosity:normal',
    '-detailedSummary:true',
    f'/p:Configuration={options["configuration"]}',
    f'/p:Platform={options["platform"]}',
  ])

  return True # TODO: parse output(s) to determine success

#-------------------------------------------------------------------------------

if __name__ == '__main__':
  build()