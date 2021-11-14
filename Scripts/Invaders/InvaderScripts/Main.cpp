#include <string>
#include "VGCVirtualGameConsole.h"
#include "Game.h"

using namespace std;

int VGCMain(const VGCStringVector &arguments) {
	Game game;
	game.run();
	return 0;
}