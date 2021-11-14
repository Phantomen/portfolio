#ifndef INCLUDED_GAME
#define INCLUDED_GAME

#include <string>
#include <vector>
#include "VGCVirtualGameConsole.h"
#include "Vectors.h"
#include "Spawner.h"
#include "HUD.h"
#include "PlayerSpaceship.h"
#include "InvaderSquare.h"
#include "Bullet.h"


class Game {
public:
	Game();
	~Game();
	void run();
private:
	void update();
	void render();
	void create();
	void destroy();
};

#endif