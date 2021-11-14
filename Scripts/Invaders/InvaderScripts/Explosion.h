#ifndef INCLUDED_EXPLOSION
#define INCLUDED_EXPLOSION

#include <string>
#include "VGCVirtualGameConsole.h"
#include "Entity.h"

class Explosion {
public:
	Explosion(VGCVector spwPos);
	virtual ~Explosion();
	virtual void update();
	virtual void render();
	static void initialize();
	static void finalize();
	virtual void destroy(Explosion *exp);

	bool getIsDead();
private:
	VGCVector mPosition;
	bool mIsDead;
	int mCurrentLife;
};

#endif