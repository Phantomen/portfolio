#include "Spawner.h"

using namespace std;

int fpsSpawn = 0;

//Vectors vectors;

double invaderBox_spawn_time_base = 8 * fpsSpawn;			//Spawn one every 4 second in the begining
double invaderBox_spawn_time_max = 2 * fpsSpawn;				//Max spawn rate is one per second
double invaderBox_spawn_time_current(invaderBox_spawn_time_base);		//Spawn one every x seconds
double invaderBox_spawn_time = 0;								//The current time
const double invaderBox_spawn_time_divider = 1.2;							//At what rate the spawn rate goes faster


Spawner::Spawner(int fps, Vectors *vec, HUD *h) :
	vector(vec), hud(h) {
	fpsSpawn = fps;

	invaderBox_spawn_time_base = 8 * fpsSpawn;
	invaderBox_spawn_time_max = 2 * fpsSpawn;				//Max spawn rate is one per second
	invaderBox_spawn_time_current = invaderBox_spawn_time_base;		//Spawn one every x seconds
	invaderBox_spawn_time = invaderBox_spawn_time_base;
}


Spawner::~Spawner() {
}

void Spawner::update() {
	if (invaderBox_spawn_time <= 0) {
		vector->addInvader(new InvaderSquare(fpsSpawn, vector, hud));

		if (invaderBox_spawn_time_current > invaderBox_spawn_time_max) {
			invaderBox_spawn_time_current = invaderBox_spawn_time_current / invaderBox_spawn_time_divider;
		}
		if (invaderBox_spawn_time_current < invaderBox_spawn_time_max) {
			invaderBox_spawn_time_current = invaderBox_spawn_time_max;
		}
		invaderBox_spawn_time = invaderBox_spawn_time_current;
	}
	else {
		invaderBox_spawn_time--;
	}
}

void Spawner::render() {
	
}

void Spawner::initialize() {
}

void Spawner::finalize() {
}

void Spawner::create() {
	vector->addPlayer(new PlayerSpaceship(fpsSpawn, vector, hud));
	vector->addInvader(new InvaderSquare(fpsSpawn, vector, hud));
}