from os import chdir
from os.path import realpath
from pathlib import Path
from subprocess import run
from shutil import rmtree

ci_dir = Path(realpath(__file__)).parent
sln_dir = ci_dir.parent
src_dir = test_dir = sln_dir.joinpath("src")
test_dir = sln_dir.joinpath("test")