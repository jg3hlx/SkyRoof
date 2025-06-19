# Frequently Asked Questions

**Q:** I downloaded SkyRoof, and my virus scanner shows an infection. Is it real?

**A:** Most likely, it is a false detection. However, it is always a good idea to test the download links **before downloading** any software. There are several online virus scanning services that you can use to check the link.
[VirusTotal](https://www.virustotal.com/gui/home/url) and [Hybrid Analysis](https://www.hybrid-analysis.com/})
are just two examples.

Copy the download link and paste it in the virus scanner page. The scanner
will download the file, test it with multiple antivirus programs, and show you the results.

In the screenshot below the download link of SkyRoof 1.5 beta was tested with VirtusTotal, and all virus scanners, except one, agreed that the file was clean. When you see something like this, you know that it was a false alarm.

If the file is clean, you can add it to the exception list of your virus scanner and safely install it. For Windows Defender follow
[these instructions](https://support.microsoft.com/en-us/windows/add-an-exclusion-to-windows-security-811816c0-4dfd-af4a-47e4-c301afe13b26#ID0EBF=Windows_11), for other anti-virus products
follow the instructions in their documentation.

![Virusyotal](../images/virus_total.png)

---

**Q:** The right part of the SkyRoof toolbar does not fit in the screen, even though the screen resolution is 1980x1280.

**A:** This happens because your **text size** setting in Windows is too high. For example, if it is set to 200%, the effective
screen width is only `1900 / 2 = 950 pixels`. To fix this, right-click on the Desktop, click on **Display Settings** and set the text size
to a lower value.

---

**Q:** How can I run two instances of SkyRoof?

**A:** By default, only one instance of SkyRoof can run at any time, but there is a work around. Make a copy of the SkyRoof.exe file in the same folder, but with a different name, e.g., SkyRoof_2.exe. Each exe will have its own settings, its own data folder, and will run independently of the other instance.
