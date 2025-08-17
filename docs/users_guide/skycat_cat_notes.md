# CAT command notes

While working on the SkyCAT engine, I discovered that the documentation for the CAT commands of many radios is incomplete and sometimes wrong.
The only way to learn the actual behavior of the CAT system is by experimentation. Below are my notes that may help the people trying to use CAT commands in their software.

## IC-9700

- **"16", "59", "00"** - this command that disables the dual watch mode sometimes returns success code **"FB"** and sometimes the error code, **"FA"**. In both cases dual watch is disabled. I could not find any pattern in its response, just be prepared to ignore the error;

- **""05"** - set the operating frequency, fails in the SAT mode if the frequency is on the same band as that of the Sub receiver. The work around is, when it fails, to send the Swap A/B command, then set the frequency again. Note that the modes of the Main and Sub receive also get swapped and need to be re-set;

- **""05"** - if Split is enabled and we are setting the frequency of the selected VFO that is on a band other than the unselected VFO band, this command disables Split. However, if another frequency change returns to the previous band, the Split gets automatically enabled! It remembered all this time that we wanted Split, and enabled it when the VFO A and B were tuned to the same band.

## FT-847

- this radio has the Split mode, and even allows it to be used in a cross-band manner, but does not have a CAT command to enable Split, nor does it even have a command to swap the VFO's.

## FT-817

- **"F7"** - the TX status returned by this command has a PTT on/off flag in its 7-th bit. This is stated in the docs, but the values used for ON and OFF are not documented. It turns out that the logic is inverted here, 0 means ON and 1 means OFF;

- this radio returns a 1-byte reply to the Set commands, but this is not documented. The returned values are different, but it is not clear what they mean.
