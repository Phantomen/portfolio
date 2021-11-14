#ifndef INCLUDED_PLAYERSPACESHIP
#define INCLUDED_PLAYERSPACESHIP

#include "Entity.h"
#include "VGCVirtualGameConsole.h"
#include <string>
#include "Vectors.h"
#include "Bullet.h"
#include "HUD.h"
#include <string>

class PlayerSpaceship : public Entity {
public:
	PlayerSpaceship(int fps, Vectors *vec, HUD *h);
	virtual ~PlayerSpaceship();
	virtual void update();
	virtual void render();
	static void initialize();
	static void finalize();
	virtual void destroy(Entity *ent);
	virtual std::string getType() const;

	virtual VGCVector getPosition() const;
	virtual int getSize() const;
private:
	VGCVector mPosition;
	Vectors *vector;
	HUD *hud;
	std::string mType = "Player";
};

#endif