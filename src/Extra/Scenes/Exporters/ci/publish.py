from _base import *
from pack import pack
from deploy import deploy

#-------------------------------------------------------------------------------

def publish():

  print('\nCI: PUBLISH\n')
  chdir(sln_dir)

  return pack() and deploy()

#-------------------------------------------------------------------------------

if __name__ == '__main__':
  publish()