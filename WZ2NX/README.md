# WZ2NX
..is a tool to convert WZ files to NX (PKG4).

## Usage
    WZ2NX /in:Data.wz /out:Data.nx /wzv:Classic /Ds /Di /wzn

### Options
 * `/in`: Path to input WZ. Required.
 * `/out`: Path to output NX. Optional; defaults to current directory with the name being the name of the WZ with the extension `.nx`
 * `/wzv`: WZ key to use. Required and must be one of `MSEA`, `KMS`, `KMST`, `JMS`, `JMST`, `EMS`, `GMS`, `GMST`, `TMS`, `BMS`, `Classic`.
 * `/wzn`: Switch. Indicates if the input WZ does not have directory names encrypted. Specify this for MSEA, BMS and Classic; other versions may require this. If in doubt, try without this switch. If it doesn't work, then try with the switch.
 * `/Ds`: Switch. Enables dumping of sounds into output NX.
 * `/Di`: Switch. Enables dumping of images into output Nx.