from initializer import *
from codegenframework import *

if __name__ == "__main__":
    _, dst, _, raw_config, config = initialize()

    with open(dst, "w") as file:
        write_header(raw_config, file)
        write_block(
            code_block(
                raw_config[NAME] + "<T>",
                "public interface",
                [
                    f"T Visit(ISmallLangNode? Parent, {i}{raw_config["node-type suffix"]} self);" for i in config
                ]
            )
            , file)