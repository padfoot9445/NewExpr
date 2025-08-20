from generatedynamicastnodesubclasses import write_header
from codegenframework import *
import yaml
from sys import argv
from nodetypegenerator import DATA
from typing import cast, Any

SECTION_KEY = "TypeData"

def get_SmallLangType_params(type_data: dict[str, Any], config: dict[str, Any]):
    name = to_csharp(type_data["name"])
    size = to_csharp(type_data["size"])
    is_pointer = to_csharp(type_data["is pointer"])
    is_number = to_csharp(type_data["is number"])
    is_collection = to_csharp(type_data["is collection"])
    is_real = to_csharp(type_data["simulates real number"])
    is_rational = to_csharp(type_data["simulates rational number"])
    max_size = to_csharp(type_data["value max size"])
    type_code = to_csharp(config["typecodes"][name])

    number_type = "None" if not is_number else ("Float" if is_real else ("Rational" if is_rational else "INt"))
    return f"BaseValue: {type_code}, Name: \"{name}\", IsRefType: {is_pointer}, Size: {size}, IsNum: {is_number}, NumberType: NumberType.{number_type}, IsCollection: {is_collection}, ValMaxSize: {max_size}"

if __name__ == "__main__":
    config_path = argv[1]
    output_dir = argv[2]
    with open(config_path) as file:
        raw_config = yaml.load(file, yaml.Loader)[SECTION_KEY]
        config = raw_config[DATA]


    with open(output_dir, "w") as file:
        write_header(raw_config, file)
        write_block(code_class(
            name = cast(str, raw_config["name"]),
            content = [
                f"private static readonly SmallLangType _{type_data["name"].capitalize()} = new SmallLangType({get_SmallLangType_params(type_data, config)});"
                "\n"
                f"public static SmallLangType {type_data["name"].capitalize()} => _{type_data["name"].capitalize()};"
                for type_data in config["type info"]
            ] + [
                code_method(
                    method_name = "CanDeclareTo",
                    content = [
                        "if(src.IsNum && dst.IsNum) return true;",
                        "return ImplicitCastTo(src, dst);"
                    ],
                    return_type = "bool",
                    parameters = [
                        "SmallLangType src",
                        "SmallLangType dst"
                    ],
                    access_modifier = "public static"
                ),
                code_method(
                    method_name = "ImplicitCastTo",
                    content = [
                        "if(dst == src) return true;",
                        "else if(src.IsNum && dst.IsNum && src.NumberType == dst.NumberType) return dst.ValMaxSize is null || src.Size <= dst.Size;"
                    ] + [
                        f"if(src == {k.capitalize()} && ({" || ".join(f"dst == {i.capitalize()}" for i in v)})) return true;" for k,v in config["implicit casts"].items() if len(v) > 0
                    ] + [
                        "return false;"
                    ],
                    return_type = "bool",
                    parameters = [
                        "SmallLangType src",
                        "SmallLangType dst"
                    ],
                    access_modifier = "public static"
                ),
                code_method(
                    method_name = "GetType",
                    content = [
                        f"if(Type == \"{i}\") return {i.capitalize()};" for i in config["typecodes"]
                    ] + [
                        "throw new ExpaException(\"unrecognized type-string\" + Type);"
                    ],
                    return_type = "SmallLangType",
                    access_modifier = "public static",
                    parameters = ["string Type"]
                ),
            ],
            modifiers = [AccessModifiers.Public]
        ), file)
