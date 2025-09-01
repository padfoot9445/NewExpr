# Build Instructions
This project relies heavily on custom source generation. As such, the following steps are required to build it after a fresh clone:

0. Ensure you have python installed
Much of the build steps are written in python. Hence, a python interpreter needs to be installed. The rest of the steps assumes it is on your path as `python3`.

You can check if you have python installed by running
```powershell
python3 --version
```

If you do not have python installed, see [here](https://www.python.org/downloads/).

1. Create a python virtual environment

Though this step is optional, it is highly recommended. This is to ensure the dependencies installed below do not contaminate your python installation.

```powershell
python3 -m venv .env
.env/bin/activate
```

2. Install PyYAML
The configuration-files for this project are written in yaml, and PyYaml is used to parse them.

```powershell
pip install PyYAML
```
or, alternatively

```powershell
python3 -m pip install PyYAML
```

3. Run your build script of choice

There are some options to build the project. All of these should be run from the root directory of the repository. Unfortunately, I have not gotten around to testing these on a linux machine yet. I suspect that the first two options will not work on a Linux machine, though I am not sure.

If you just want to build the project:

```powershell
.\build.ps1 --no-format
```

If you want to regenerate the generated source

```powershell
.\regen
```

If the first two options fail (probably because these are powershell scripts and afaik most Linux distributions do not run powershell out of the box):

```bash
python3 ./Common/build.py ./ ./NewExpr.yaml
```

with optional arguments:
- `--no-dotnet` only regenerates generated source without building
- `--no-format` does not run dotnet format (recommended)
- `--clean` also removes any folders instead of just removing files during cleanup