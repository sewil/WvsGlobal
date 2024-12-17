import os
import shutil
import glob

def apply_patch(patch_file, old_file):
    cmd=f'..\\Patching\\courgette64.exe -apply "{old_file}" "{patch_file}" "{patch_file}_Data.wz"'
    print(cmd)
    os.system(cmd)
    os.remove(old_file)
    return f"{patch_file}_Data.wz"

original_file = "Data.wz"
shutil.copy(original_file, f'{original_file}.bak')
patched_file = original_file
for patch_file in glob.glob("Patches\\*.patch"):
    print(patch_file)
    patched_file = apply_patch(patch_file, patched_file)

shutil.move(patched_file, "Patches\Data.wz")
