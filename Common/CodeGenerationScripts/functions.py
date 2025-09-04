from codegenframework import *
from typing import Any

def get_params(function_data: Any) -> str:
    return f"Name: \"{function_data["name"]}\", ID: new({function_data["ID"]}), RetVal: GenericSmallLangType.ParseType(\"{function_data["returns"]}\"), ArgTypes: [{", ".join(f"GenericSmallLangType.ParseType(\"{i}\")" for i in function_data["arguments"])}]"

if __name__ == "__main__":
    config_path, output_dir, _, raw_config, config = initialize()
    class_name = raw_config["name"]

    with open(output_dir, "w") as file:
        write_header(raw_config, file)
        write_block(
            code_class(
                name = class_name,
                content = [
                    f"public static FunctionSignature<BackingNumberType, GenericSmallLangType>{i["name"]} {{ get; }} = new({get_params(i)});" for i in config
                ] + [
                    f"public static IEnumerable<FunctionSignature<BackingNumberType, GenericSmallLangType>> StdLibFunctions {{ get; }} = [{", ".join(i["name"] for i in config)}];"
                ],
                modifiers = ["partial"]
            )
            ,file
        )
    
