# Waterfall Display

The waterfall display and associated
[Frequency Scale](frequency_scale.md)
is the central piece of SkyRoof that integrates most of the functions available in the application:

![Waterfall Display panel](../images/waterfall.png)

The waterfall spans over 3 MHz of spectrum (depending on the SDR model) so that it covers the whole
satellite segment, 435-438 MHz, on the 70 cm band. On the 2 m band the satellite segment is
only 200 kHz wide, 145.8-146 MHz, so it also fits completely in the waterfall.

- Zoom in and out using the mouse wheel
- Pan by dragging the waterfall horizontally with your mouse

A mouseclick on the waterfall display:

- tunes the SDR and external radio to a terrestrial signal
- or, if the frequency is within the transponder segment of a passing satellite, selects
    that satellite and sets the transponder offset to the clicked signal.

A click on the **Sliders** button in the top left corner of the panel opens
the sliders that adjust brightness, contrast and scrolling speed of the waterfall,
and select a color palette:

![Waterfall Sliders](../images/waterfall_sliders.png)

## See Also

- [Frequency Scale](frequency_scale.md)
- [Doppler Tracking](doppler_tracking.md)