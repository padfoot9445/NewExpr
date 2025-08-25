from pathlib import Path
from codegenframework import *
from initializer import *
from typing import Callable

CS_EXTENSION = ".cs"

if __name__ == "__main__":
    config_path, output_dir, section_name, raw_config, config = initialize()

    names = raw_config["name"]
    enum_name = names["enum"]
    record_name = names["record"]
    wrapper_name = enum_name + "Wrapper"

    file_path_enum = Path(output_dir)/(enum_name + CS_EXTENSION)
    file_path_record = Path(output_dir)/(record_name + CS_EXTENSION)
    file_path_wrapper = Path(output_dir)/(wrapper_name + CS_EXTENSION)


    enum_content: list[str] = []
    record_content: list[str] = []


    #create enum and record classes
    for operation in config:
        op_name = operation["name"]
        op_args: list[dict[str, str]] = operation["arguments"]
        comment = operation["comment"]

        #enum
        enum_content.append(
            f"{op_name}, /*{comment}*/"
        )
        

        #all the facotry methods
        method_content: list[str] = []
        method_params: list[str] = []
        other_params: list[str] = []
        other_content: list[str] = []
        other_call_params: list[str] = []
        generics = 0
        for argument in op_args:
            generics += 1
            type = f"T{generics}"
            method_params.append(
                f"NumberWrapper<{type}, BackingNumberType> {argument["name"]}"
            )
            other_params.append(
                f"{type} {argument["name"]}"
            )
            other_call_params.append(
                f"(NumberWrapper<{type}, BackingNumberType>){argument["name"]}"
            )

        method_content.append(
            f"return new {record_name}({enum_name}.{op_name}" +
            (f", {", ".join(f"{i["name"]}" for i in op_args)}" if len(op_args) > 0 else "") +
            ");"
        )

        other_content.append(
            f"return {op_name}({", ".join(other_call_params)});"
        )
        generics_string = f"<{", ".join(f"T{i}" for i in range(1, generics + 1))}>" if generics > 0 else ""
        
        get_where: Callable[[int], str] = lambda x: f"where T{x} : IBinaryInteger<T{x}>, IMinMaxValue<T{x}>"


        get_affixes: Callable[[list[str]], list[str]] = lambda x: [
                    generics_string,
                    f"({", ".join(x)})"
                ] + [
                    get_where(i) for i in range(1, generics + 1)
                ]
        modifiers: list[str | AccessModifiers] = ["public", "static"]

        record_content.append(
            code_block(
                name=op_name,
                keyword=record_name,
                content=method_content,
                affixes=get_affixes(method_params),
                modifiers=modifiers
            )
        )
        if len(other_params) > 0:
            record_content.append(
                code_block(
                    name=op_name,
                    keyword=record_name,
                    content=other_content,
                    affixes=get_affixes(other_params),
                    modifiers=modifiers
                )
            )


    #write enum
    with open(file_path_enum, "w") as file:
        write_header(raw_config, file)
        write_block(code_block(
            name=enum_name,
            keyword="enum",
            content=enum_content,
            modifiers=[AccessModifiers.Public],
            affixes=[":", "HighLevelOpcodeBackingType"]
        ), file)
    
    with open(file_path_record, "w") as file:
        write_header(raw_config, file)
        write_block(code_class(
            name=record_name,
            content=record_content + [
                f"public new {enum_name} Op {{ get; init; }}",
            ],
            modifiers=[AccessModifiers.Public, "record"],
            ctors=[
                code_ctor(
                    class_name=record_name,
                    content=[
                        "this.Op = Op;"
                    ],
                    access_modifier="private",
                    parameters=[
                        f"{enum_name} Op",
                        f"params IEnumerable<IOperationArgument<BackingNumberType>> Operands"
                    ],
                    delegated_ctor="base",
                    delegated_ctor_arguments=[
                        f"({wrapper_name})Op",
                        f"Operands.ToArray()"
                    ]
                )
            ],
            parents=[f"Operation<{enum_name}, BackingNumberType>"]
        ), file)

    #write opcode wrapper
    with open(file_path_wrapper, "w") as file:
        file.write(
            f"""
using Common.LinearIR;

namespace SmallLang.IR.LinearIR;

public record class {wrapper_name}({enum_name} Op) : GenericOperationArgument<BackingNumberType, {enum_name}>(Op)
{{
    protected override IEnumerable<BackingNumberType> GetFromOp({enum_name} op)
    {{
        HighLevelOpcodeBackingType NumVal = (HighLevelOpcodeBackingType)op;
        int BITS = (int)Math.Ceiling(Math.Log2(HighLevelOpcodeBackingType.MaxValue));
        int OutBits = (int)Math.Ceiling(Math.Log2(BackingNumberType.MaxValue));
        for (int i = OutBits; i <= BITS; i += OutBits)
        {{
            yield return (BackingNumberType)((NumVal >> (BITS - i)) & BackingNumberType.MaxValue);
        }}
    }}
    public static implicit operator {wrapper_name}({enum_name} inp)
    {{
        return new(inp);
    }}
    public override string ToString()
    {{
        return Op.ToString();
    }}
}}
"""
        )

