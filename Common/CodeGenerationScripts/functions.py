import yaml
from sys import argv
from typedata import DATA
from generatedynamicastnodesubclasses import write_header
from codegenframework import *
from typing import Any

SECTION_KEY = "Functions"
def get_params(function_data: Any) -> str:
    return f"Name: \"{function_data["name"]}\", ID: new({function_data["ID"]}), RetVal: TypeData.{function_data["returns"].capitalize()}, ArgTypes: [{", ".join(i.capitalize() for i in function_data["arguments"])}]"

if __name__ == "__main__":
    config_path = argv[1]
    output_dir = argv[2]
    with open(config_path) as file:
        raw_config = yaml.load(file, yaml.Loader)[SECTION_KEY]
        config = raw_config[DATA]
        name = config["name"]

    with open(output_dir, "w") as file:
        write_header(raw_config, file)
        write_block(
            code_class(
                name = name,
                content = [
                    code_ctor(
                        class_name = name,
                        content = [
                            "Values = new Functions();"
                        ] + [
                            f"Values.RegisterFunction({i["name"]});"
                            for i in config["data"]
                        ],
                        access_modifier = "static"
                    )
                ] + [
                    f"public static FunctionSignature<BackingNumberType, SmallLangType>{i["name"]} {{ get; init; }} = new({get_params(i)});" for i in config["data"]
                ],
                modifiers = ["partial"]
            )
            ,file
        )
    
