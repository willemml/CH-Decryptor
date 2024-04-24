# CH-Decryptor

Dumps the data from Clone Hero's built in songs so they can be used in other Rhythm games like YARG.

Requires [installing BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html).

## Usage

Clone Hero uses stripped DLLs, to fix this, follow [this guide](https://hackmd.io/@ghorsington/rJuLdZTzK).
Clone Hero uses Unity version 2022.3.11, download [mscorlibs here](https://unity.bepinex.dev/corlibs/2022.3.11.zip),
and [UnityEngine here](https://unity.bepinex.dev/libraries/2022.3.11.zip). BepInEx (and therefore this program)
will not work without replacing the DLLs with their unstripped counterparts.

Once BepInEx is installed, install my patcher plugin ([CH-Patcher](https://github.com/willemml/CH-Patcher)). 
Then drop `Decryptor.dll` (which you can get from the GitHub releases) into the BepInEx plugins folder.

Then, open the game. It should exit after a couple seconds and the dumped charts will be located in
a folder named `charts` (these will be in the same folder as the game executable).

## Development

To compile this plugin you will need to create a `lib` folder in the repo. From the game files,
take `Assembly-CSharp.dll` and put it into the lib folder. Then run `dotnet build`.
