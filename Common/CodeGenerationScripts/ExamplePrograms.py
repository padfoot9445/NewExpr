from codegenframework import *
from initializer import *
import os


if __name__ == "__main__":
    _, output_dir, _, raw_config, config = initialize()

    with open(output_dir/"ExamplePrograms.cs", "w") as file:
        write_header(raw_config, file)
        class_content: list[str] = []
        program_names: list[str] = []

        for root, dirs, files in os.walk(Path(config["programs path"]).resolve()):
            for program_file in files:
                if os.path.splitext(program_file)[1] in config["extensions"]:
                    with open(Path(root)/program_file) as opened_program_file:
                        program_name = os.path.splitext(program_file)[0]
                        program_names.append(program_name)

                        class_content.append(f"public static readonly string {program_name} = @\"\n{"\n".join(opened_program_file.readlines())}\n\";")
        
        class_content.append("public static string[] AllPrograms => __AllPrograms.Select(x => x).ToArray();")
        class_content.append(f"private static readonly string[] __AllPrograms = [{", ".join(program_names)}];")

        write_block(code_class(
            name="ExamplePrograms",
            content=class_content,
            modifiers=["public", "static"]
        ), file)