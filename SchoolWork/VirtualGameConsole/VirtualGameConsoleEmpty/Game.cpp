#include "Game.h"

using namespace std;



//Game
static const string name			  = "Invaders";
static const int	display_width			  = 450;
static const int	display_height			  = 600;
static const int frames_per_second = 60;
static const double update_time		  = 1 / frames_per_second;


Vectors vec;
HUD hud;
Spawner spawn(frames_per_second, &vec, &hud);




Game::Game() {
	VGCVirtualGameConsole::initialize(name, display_width, display_height);
	hud.initialize();
	//spawn.initialize();
	vec.initialize();
	PlayerSpaceship::initialize();
	InvaderSquare::initialize();
	Bullet::initialize();
}


Game::~Game() {
	//InvaderBox::finalize();
	//PlayerSpaceship::finalize();
	//spawn.finalize();
	vec.finalize();
	hud.finalize();
	PlayerSpaceship::finalize();
	InvaderSquare::finalize();
	Bullet::finalize();
	//VGCVirtualGameConsole::finalize();
}

void Game::run() {
	create();
	VGCTimer timer = VGCClock::openTimer(update_time);
	while (VGCVirtualGameConsole::beginLoop() && hud.getHealth() > 0) {

		VGCClock::reset(timer);
		update();
		if (VGCDisplay::beginFrame()) {
			VGCColor backgroundColor(255, 0, 0, 0);
			VGCDisplay::clear(backgroundColor);
			render();
			VGCDisplay::endFrame();
		}
		VGCVirtualGameConsole::endLoop();
		VGCClock::wait(timer);
	}
	VGCClock::closeTimer(timer);
	destroy();
}

void Game::update() {
	spawn.update();
	vec.update();
	hud.update();
}

void Game::render() {
	vec.render();
	hud.render();
}

void Game::create() {
	spawn.create();
}

void Game::destroy() {
	vec.destroy();
}