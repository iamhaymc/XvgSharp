from _base import *

#-------------------------------------------------------------------------------

def deploy():

  print('\nCI: DEPLOY\n')
  chdir(sln_dir)

  if not 'nuget_source' in secrets:
    raise Exception(f'"nuget_source" not found in {secrets_file.absolute()}')
  
  if not 'nuget_api_key' in secrets:
    raise Exception(f'"nuget_api_key" not found in {secrets_file.absolute()}')
  
  nupkg_paths = list(dist_dir.rglob('*.nupkg'))
  if len(nupkg_paths) == 0:
    raise Exception(f'"NuGet package(s) not found in {dist_dir.absolute()}')
  
  for nupkg_path in nupkg_paths:
    result = run(shell=True, capture_output=False, args=[
      'dotnet', 'nuget', 'push', 
      nupkg_path,
      '-s',
      secrets['nuget_source'],
      '-k',
      secrets['nuget_api_key'],
      '--skip-duplicate',
    ])

  return True # TODO: parse output(s) to determine success

#-------------------------------------------------------------------------------

if __name__ == '__main__':
  deploy()