from _base import *

#-------------------------------------------------------------------------------

def format():

  print('\nCI: FORMAT\n')
  chdir(sln_dir)
  
  # result = run(shell=True, capture_output=False, args=[
  #   options['astyle_exe'],
  #   '--align-pointer=name',
  #   '--align-pointer=type',
  #   '--align-reference=name',
  #   '--attach-closing-while',
  #   '--break-return-type-decl',
  #   '--break-return-type',
  #   '--convert-tabs',
  #   '--delete-empty-lines',
  #   '--indent-preproc-define',
  #   '--indent=spaces=2',
  #   '--keep-one-line-blocks',
  #   '--lineend=linux',
  #   '--pad-comma',
  #   '--pad-header',
  #   '--pad-oper',
  #   '--remove-braces',
  #   '--squeeze-lines=1',
  #   '--squeeze-ws',
  #   '--style=google',
  #   '--suffix=none',
  #   '--unpad-brackets',
  #   '--unpad-paren',
  #   '--recursive',
  #   '*.c,*.cc,*.cpp,*.h,*.hpp'
  # ])

  # print()

  result = run(shell=True, capture_output=False, args=[
    'dotnet', 'format', '-v', 'normal'
  ])

  return True # TODO: parse output(s) to determine success

#-------------------------------------------------------------------------------

if __name__ == '__main__':
  format()