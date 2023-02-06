import re
import argparse
import os

alpha = "abcdefghijklmnopqrstuvwxyz"
ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

# Argparse
parser = argparse.ArgumentParser(description='Convert script file to .cs')
parser.add_argument('input', type=str)
args = parser.parse_args()

output = [
    "using System;",
    "using WvsBeta.Game;",
    "using WvsBeta.Game.Scripting;",
    "",
    "namespace WvsBeta.Scripts.Scripts",
    "{"
]
with open(args.input) as f:
    lines = f.read().splitlines()
    nLine=0
    script_name = None
    vars = set()
    script_index = 0
    for line in lines:
        line = line.replace("\t", "    ")
        line = line.replace("registerTransferField", "target.ChangeMap")
        line = line.replace(" = random(", " = Rand32.NextBetween(")
        line = line.replace(".n", ".")
        line = line.replace(" = currentTime;", " = MasterThread.CurrentDate;")
        line = line.replace("integer( substring( cTime, 13, 1 ) )", "cTime.Minute")
        if re.match(r'\s+(else )?if\s+\(.*( and | or ).*\)\s+\{?', line):
            line = line.replace(" or ", " || ").replace(" and ", " && ")
        # if (target.Level > 29 and target.Level < 40 ) {
        for idx, x in enumerate(alpha):
            a = alpha[idx]
            A = ALPHA[idx]
            line = line.replace("." + a, "." + A)
        if script_name is not None:
            if line == "}": # Close script
                output.insert(script_index, "        dynamic " + ", ".join(vars) + ";") # Insert vars
                print("Close " + script_name)
                vars.clear()
                script_name = None
                output.append("        }")
                output.append("    }")
            else:
                var_m = re.search(r'\s+([a-zA-Z\d]+) = ', line)
                if var_m is not None: # Var init
                    var_name = var_m.group(1)
                    if (var_name in vars) == False:
                        vars.add(var_name)
                        print(var_name)
                        # line = line.replace(var_name, "var " + var_name)
                output.append("        " + line)
        cmt_m = re.match(r'^\/\/.+', line) # Script comment
        if cmt_m is not None:
            scr_cmt = cmt_m.group(0)
            cmt_line = nLine
        scr_m = re.match(r'script "([a-z_\d]+)"', line)
        if scr_m: # Open script
            script_name = scr_m.group(1)
            print("\r\nOpen " + script_name + "")
            if cmt_line == nLine - 1:
                output.append("    " + scr_cmt)
            output.append("    [Script(\"" + script_name + "\")]")
            output.append("    class " + script_name + " : INpcScript\n    {")
            script_index = len(output)
            output.append("        public void Run(INpcHost self, GameCharacter target)\n        {")
        nLine+=1

output.append("}")
output_s = "\n".join(output)
# print(output_s)

file_name, ext = os.path.splitext(args.input)
file_name_out = file_name + ".cs"
f_out = open(file_name_out, "w")
f_out.write(output_s)
f_out.close()

print("Output saved to " + file_name_out)