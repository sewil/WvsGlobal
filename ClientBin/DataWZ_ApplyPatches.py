import os
import shutil
import glob

def apply_patch(patch_file, old_file):
    cmd=f'..\\Patching\\courgette64.exe -apply "{old_file}" "{patch_file}" "{patch_file}_Data.wz"'
    # print(cmd)
    output = os.system(cmd)
    if (output == 1):
        raise ValueError()
    os.remove(old_file)
    return f"{patch_file}_Data.wz"

original_file = "Data.wz"
shutil.copy(original_file, f'{original_file}.bak')
patched_file = original_file
for patch_file in glob.glob("Patches\\*.patch"):
    patch_file_name = os.path.basename(patch_file)
    print('\napplying patch ' + patch_file_name + '...')
    try:
        new_patched_file = apply_patch(patch_file, patched_file)
        patched_file = new_patched_file
        print('patch applied')
    except:
        print('patch failed')
        continue

shutil.move(patched_file, "Data.wz")
