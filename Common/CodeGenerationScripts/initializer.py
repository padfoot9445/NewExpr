import sys
import os
import yaml
from typing import Literal
from pathlib import Path
USINGS: str = "usings"
NAMESPACE: str = "namespace"
NAME: Literal["name"] = "name"
CHILDREN: Literal["children"] = "children"
DATA: Literal["data"] = "data"

def initialize():
    """returns: config_path, output_dir, section_key, raw_config, config"""
    
    config_path = Path(sys.argv[1])
    output_dir = Path(sys.argv[2])
    section_key = sys.argv[3]
    cwd = sys.argv[4]

    os.chdir(cwd)

    with open(config_path) as file:
        raw_config = yaml.load(file, yaml.Loader)[section_key]
        config = raw_config[DATA]
    return config_path, output_dir, section_key, raw_config, config
