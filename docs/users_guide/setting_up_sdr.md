# Setting Up SDR

## Supported Radios

SkyRoof uses the
[Soapy SDR](https://github.com/pothosware/SoapySDR)
engine to interface with the SDR radios. Currently it supports:

- Airspy Mini;
- SDRplay;
- RTL-SDR.

> [!NOTE]
> It may be possible to add support of other SDR devices to SkyRoof. Contact me if you have an unsupported SDR
> and are willing to do extensive testing.

If you plan on using one of the SDRplay radios with SkyRoof, download the latest SDRplay API from
[their web site](https://www.sdrplay.com/api/) and install it on your system.

## Selecting an SDR device

Connect your SDR device to the computer, then click on **Tools / SDR Devices** in the main menu. This will open the
**SDR Devices dialog**:

![SDR Devices dialog](../images/sdr_devices_dialog.png)

All active SDR devices are listed on the left panel. Click on the one that you want to use.

## Configuring the device

The right panel shows all settings that the device driver understands. The setting names and descriptions (shown on the
bottom panel) come from the driver, with two exceptions described below. For information about these
settings see the documentation that comes with the radio.

The two settings, common to all radios, are:

- **PPM** - the correction factor for the SDR clock frequency, expressed in parts per million.
  This setting is important for the correct operation of the Doppler tracking algorithm, see the
  [Calibrating PPM Correction](calibrating_ppm_correction.md) section for details;

- **Single Gain** - when set to true, the SDR gain is controlled by the **RF Gain** slider on the toolbar.
  This is the recommended setting. When it is set to false, the settings in the **Stage Gains** are applied to the
  individual stages of the SDR, and the gain slider is disabled.
