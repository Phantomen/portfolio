#ifndef INCLUDED_INVADERSQUARE
#define INCLUDED_INVADERSQUARE

#include "Explosion.h"
#include "Entity.h"
#include "VGCVirtualGameConsole.h"
#include <string>
#include "Vectors.h"
#include "Bullet.h"
#include "HUD.h"

class InvaderSquare : public Entity {
public:
	InvaderSquare(int fps, Vectors *vec, HUD *h);
	virtual ~InvaderSquare();
	virtual void update();
	virtual void render();
	static void initialize();
	static void finalize();
	virtual void destroy(Entity *ent);
	virtual std::string getType() const;

	VGCVector getPosition() const;
	int getSize() const;
private:
	VGCVector mPosition;
	Vectors *vector;
	HUD *hud;
	int mSpeedX, mSpeedY, mDir, mTimeBetweenShots;
	std::string mType = "Invader";
};

#endif