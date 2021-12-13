#ifndef INCLUDED_BULLET
#define INCLUDED_BULLET

#include "VGCVirtualGameConsole.h"
#include "Entity.h"
#include <string>

class Bullet : public Entity {
public:
	Bullet(VGCVector shotOriginPosition, int speedY, int speedX);
	~Bullet();
	void update();
	void render();
	static void initialize();
	static void finalize();
	VGCVector getPosition() const;
	int getSize() const;
	void destroy(Entity *bullet);
	std::string getType() const;
private:
	VGCVector mPosition;
	int mSpeedX, mSpeedY;
	std::string mType = "Bullet";
};

#endif