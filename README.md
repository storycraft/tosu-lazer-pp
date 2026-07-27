# Tosu lazer pp calculator
osu!lazer Node.js napi binding with various utilities for calculating performance points and difficulty.

## Features
* Highly efficient gradual difficulty attribute calculation and beatmap subsetting.
* Performance point calculator.
* Skill strains for graph visualization.
* Lazer mod parsing with json settings.
* Beatmap gamemode conversion.
* Hit results simulator.
* Accuracy calculator.
* Calculate Beatmap difficulty with/without mods applied.
* Calculate hit windows for given beatmap and mods.

## Credits
 * [osu-tools](https://github.com/ppy/osu-tools): Score simulator and accuracy calculation code.
  
## Development
### Project Structure
* `lib/vendor`: Vendored osu!lazer repository with local patches applied.
* `lib/patches`: osu!lazer local patches for gradual calculation and skill strains.
* `lib/native`: Main source code.

### Requirements
* Node.js >= 24
* Git
* dotnet SDK >= 10.0
* pnpm

### Setup
1. Run the following command to setup lazer and install dependencies:
```bash
pnpm install
```

2. Run the following command to build the project:
```bash
pnpm build
```

## Scripts
* Updating vendored osu!lazer repository:
```bash
pnpm set-vendor <repository_url> <branch or commit hash>
```
You can also use `update-vendor` Github Actions.

* Creating local patches in osu!lazer:
```bash
pnpm create-patch
```
To create a patch, create changes and make commits in `lib/vendor` and run the command above.
The created patch is in `lib/patches` directory.

## License
This project is licensed under the LGPL-3.0 License.
See the [LICENSE](LICENSE) file for details.
