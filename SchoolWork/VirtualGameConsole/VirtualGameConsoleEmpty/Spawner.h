#ifndef INCLUDED_SPAWNER
#define INCLUDED_SPAWNER

#include <string>
#include "VGCVirtualGameConsole.h"
#include "PlayerSpaceship.h"
#include "InvaderSquare.h"
#include "Entity.h"
#include "Vectors.h"
#include "HUD.h"

class Spawner {
public:
	Spawner(int fps, Vectors *vec, HUD *h);
	~Spawner();
	void update();
	void render();
	static void initialize();
	static void finalize();

	void create();
private:
	Vectors *vector;
	HUD *hud;
};

#endif