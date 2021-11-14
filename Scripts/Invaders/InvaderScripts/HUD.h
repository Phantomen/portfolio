#ifndef INCLUDED_HUD
#define INCLUDED_HUD


#include <string>
#include <fstream>
#include "VGCVirtualGameConsole.h"
#include "Entity.h"


class HUD {
public:
	HUD();
	virtual ~HUD();
	virtual void update();
	virtual void render();
	void initialize();
	void finalize();
	void giveDamage(int dmg);
	int getHealth();
	void giveScore(int scr);

private:
	int mHealth, mScore, mHighScore;
	VGCFont mFont;
	std::ifstream mInput;
	std::ofstream mOutput;

	void printNewHighScore();
	void updateStrings();
};

#endif