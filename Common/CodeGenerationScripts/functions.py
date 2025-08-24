from codegenframework import *
from typing import Any

def get_params(function_data: Any) -> str:
    return f"Name: \"{function_data["name"]}\", ID: new({function_data["ID"]}), RetVal: TypeData.{function_data["returns"].capitalize()}, ArgTypes: [{", ".join(f"TypeData.{i.capitalize()}" for i in function_data["arguments"])}]"

if __name__ == "__main__":
    config_path, output_dir, _, raw_config, config = initialize()
    class_name = raw_config["name"]

    with open(output_dir, "w") as file:
        write_header(raw_config, file)
        write_block(
            code_class(
                name = class_name,
                content = [
                    code_ctor(
                        class_name = class_name,
                        content = [
                            "Values = new Functions();"
                        ] + [
                            f"Values.RegisterFunction({i["name"]});"
                            for i in config
                        ],
                        access_modifier = "static"
                    )
                ] + [
                    f"public static FunctionSignature<BackingNumberType, SmallLangType>{i["name"]} {{ get; }} = new({get_params(i)});" for i in config
                ],
                modifiers = ["partial"]
            )
            ,file
        )
    
