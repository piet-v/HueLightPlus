# HueLight+
Ambilight-clone for NZXT HUE+
[Latest Release](https://github.com/piet-v/HueLightPlus/releases/latest)

# Showcase video
[https://www.youtube.com/watch?v=hn5HEN5PzPE](https://www.youtube.com/watch?v=hn5HEN5PzPE)

# Features
* Ambilight
* High fps LED color updates (depending on CPU + resolution)
* Supports max 1 Hue+
* Supports max 2 Channels
* Supports max 4 LED strips / channel
* Supports config.json to configure before launch

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
* A Config.json file can be used to control certain settings.
  * bool startsHidden;
  * double gamma;
  * byte delay
  * int scanDepth
  * int pixelsToSkipPerCoordinate
  * String devices.port
  * int Screenregion[side].leds.channel
  * int Screenregion[side].leds.number

# Todo
* Configure through UI
* Support more than 1 HUE+
* Multi monitor
* Turn off LEDs after closing the software
* (Maybe) NZXT Kraken Support
* (Maybe) NZXT AER FAN Support

# Bugs
* Feel free to post issues on this github page

# Credits
* [h0uri](http://www.instructables.com/member/h0uri/) whos C# Ambilight algorithm was way faster and thus got used as the basis for this.
* [kusti8](https://github.com/kusti8/hue-plus) whos project contained an example of the reverse engineered Hue+ protocol
