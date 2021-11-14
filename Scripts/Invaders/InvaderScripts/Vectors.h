#ifndef INCLUDED_VECTORS
#define INCLUDED_VECTORS

#include "VGCVirtualGameConsole.h"
#include "Entity.h"
#include <vector>
#include "Explosion.h"


class Vectors {
public:
	Vectors();
	~Vectors();
	void update();
	virtual void render();
	static void initialize();
	void finalize();
	void addPlayer(Entity *player);
	void addBulletPlayer(Entity *bullet);
	void addBulletInvaders(Entity *bullet);
	void addInvader(Entity *invader);
	void addExplosion(Explosion *explosion);

	void destroy();
private:
	typedef std::vector<Entity*> EntityVector;
	EntityVector mPlayer, mInvaders, mBulletsPlayer, mBulletsInvaders;
	typedef std::vector<Explosion*> ExplosionVector;
	ExplosionVector mExplosions;

	void checkCollision(EntityVector::size_type startIndex, EntityVector &entVec1, EntityVector &invVec2);
	void checkCollisionPlayer(EntityVector::size_type startIndex, EntityVector &playVec, EntityVector &entVec);
	void checkExplosions(ExplosionVector::size_type startIndex, ExplosionVector &xploVec);
	void checkBulletOutOfBounds(EntityVector::size_type startIndex, EntityVector &bulVec);
};

#endif