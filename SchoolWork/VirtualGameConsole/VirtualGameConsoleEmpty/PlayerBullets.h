#ifndef INCLUDED_ASTEROID
#define INCLUDED_ASTEROID

#include <string>
#include "VGCVirtualGameConsole.h"
#include "Entity.h"
#include "PlayerSpaceship.h"

class PlayerBullet : public Entity {
public:
	PlayerBullet();
	virtual ~PlayerBullet();
	virtual void update();
	virtual void render();
	static void initialize();
	static void finalize();
	const VGCVector getBulletPosition();
	static int getBulletSize;
private:
	VGCVector mPosition;
};

#endif