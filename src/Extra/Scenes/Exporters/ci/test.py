from _base import *

#-------------------------------------------------------------------------------

def test():

  print('\nCI: TEST\n')
  chdir(sln_dir)
  
  result = run(shell=True, capture_output=False, args=[
    options['msbuild_exe'],
    options['solution'],
    '-restore',
    f'-maxCpuCount:{options["maximum_cpus"]}',
    '-verbosity:quiet',
    '-detailedSummary:false',
    f'/p:Configuration=Debug',
    f'/p:Platform={options["platform"]}',
  ])
  
  print()

  result = run(shell=True, capture_output=False, args=[
    options['vstest_exe'],
    options['test_assembly'],
  ])

  return True # TODO: parse output(s) to determine success

#-------------------------------------------------------------------------------

if __name__ == '__main__':
  test()