from pathlib import Path
from os.path import realpath
from os import chdir
from subprocess import run
from shutil import rmtree
from json import loads as fromjson

#-------------------------------------------------------------------------------

ci_dir = Path(realpath(__file__)).parent
sln_dir = ci_dir.parent
dist_dir = sln_dir.joinpath('dist')

options_file = ci_dir.joinpath('_options.json')
options = fromjson(options_file.read_text()) if options_file.exists() else {}

secrets_file = ci_dir.joinpath('_secrets.json')
secrets = fromjson(secrets_file.read_text()) if secrets_file.exists() else {}