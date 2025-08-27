from codegenframework import *
from initializer import *
import os


if __name__ == "__main__":
    _, output_dir, _, raw_config, config = initialize()

    with open(output_dir/"ExamplePrograms.cs", "w") as file:
        write_header(raw_config, file)
        class_content: list[str] = []
        for root, dirs, files in os.walk(Path(config["programs path"]).resolve()):
            for program_file in files:
                if os.path.splitext(program_file)[1] in config["extensions"]:
                    with open(Path(root)/program_file) as opened_program_file:
                        class_content.append(f"public static readonly string {os.path.splitext(program_file)[0]} = @\"\n{"\n".join(i for i in opened_program_file.readlines())}\n\";")
        
        write_block(code_class(
            name="ExamplePrograms",
            content=class_content,
            modifiers=["public", "static"]
        ), file)