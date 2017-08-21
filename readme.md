# HueLight+

Ambilight-clone for NZXT HUE+

# Features
* Ambilight-clone
* High fps LED color updates (depending on CPU + resolution)
* Supports 1 HUE+ with a max of 2 channels
* Each channel kan have up to 4 LED strips

# Requirements
* Windows
* [NZXT HUE+](https://www.nzxt.com/products/hue-plus)
* [DFMirage Mirror Driver](http://www.demoforge.com/dfmirage.htm)

# Assumptions
* Current version assumes the LEDS strips are mounted as follows:

* x ← ← TOP ← ←x
* ↓                    ↑ 
* ↓                    ↑ 
* ↓                    ↑ 
* L                    R
* E                    I
* F                    G
* T                    H
* ↓                    T 
* ↓                    ↑ 
* ↓                    ↑ 
* x ← BOTTOM ← x ← ← ← ← ← ← HUE+

* Channel 1: (Right (10 leds) => Top (20 leds) => Left (10 leds))
** Right: bottom-right => top-right
** Top: top-right => top-left
** Left: top-left => bottom-left
* Channel 2 (Bottom (20 leds)
** Bottom: bottom-left => bottom-right

# Configuration
* A Config.ini file can be used to control certain settings. Put each value on a line without delimiters!
  * bool startsHidden;
  * double gamma;
  * int rightLedCount
  * int leftLedCount
  * int topLedCount
  * int bottomLedCount
  * String huePlusPort
  * int baudRate
  * byte delay
  * int scanDepth
  * int pixelsToSkipPerCoordinate

# Todo
* Make .exe downloadable somewhere
* Better configuration for LED setup
* Support more than 1 HUE+
* Turn off LEDs after closing the software

# Bugs
* Feel free to post issues on this github page

# Showcase video
[https://www.youtube.com/watch?v=hn5HEN5PzPE](https://www.youtube.com/watch?v=hn5HEN5PzPE)

# Credits
* [h0uri](http://www.instructables.com/member/h0uri/) whos C# Ambilight algorithm was way faster and thus got used as the basis for this.
* [kusti8](https://github.com/kusti8/hue-plus) whos project contained an example of the reverse engineered Hue+ protocol