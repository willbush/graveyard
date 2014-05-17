package kiloboltgame;

import java.util.ArrayList;

public class Robot {
    private static final int JUMP_SPEED = -15;
    private static final int WALK_SPEED = 5;
    private static final int GROUND_POS = 382;
    private static final int STARTING_POS = 100;
    private static final int START_SCROLLING_POS = 390;

    private int robotPositionX = STARTING_POS;
    private int robotPositionY = GROUND_POS;

    private int robotSpeedX = 0;
    private int robotSpeedY = 0;

    private boolean jumped = false;
    private boolean movingLeft = false;
    private boolean movingRight = false;
    private boolean ducked = false;

    private static Background bg1 = StartingClass.getBg1();
    private static Background bg2 = StartingClass.getBg2();

    private ArrayList<Projectile> projectiles = new ArrayList<Projectile>();

    public void update() {
        handleWalking();
        handleBackgroundScrolling();
        handleJumping();
        preventRobotMovingOutOfFrame();
    }

    private void handleWalking() {
        if (robotSpeedX < 0) {
            robotPositionX += robotSpeedX;
        }
        if (robotPositionX <= START_SCROLLING_POS && robotSpeedX > 0) {
            robotPositionX += robotSpeedX;
        }
    }

    private void handleBackgroundScrolling() {
        if (robotSpeedX == 0 || robotSpeedX < 0) {
            bg1.setBgSpeedX(0);
            bg2.setBgSpeedX(0);
        }
        if (robotSpeedX > 0 && robotPositionX > START_SCROLLING_POS) {
            bg1.setBgSpeedX(-WALK_SPEED);
            bg2.setBgSpeedX(-WALK_SPEED);
        }
    }

    private void handleJumping() {
        if (jumped) {
            whatGoesUpMustComeDown();
            if (robotHasLanded()) {
                robotPositionY = GROUND_POS;
                robotSpeedY = 0;
                jumped = false;
            }
        }
    }

    private void whatGoesUpMustComeDown() {
        robotSpeedY += 1;
        robotPositionY += robotSpeedY;
    }

    private boolean robotHasLanded() {
        return robotPositionY + robotSpeedY >= GROUND_POS;
    }

    private void preventRobotMovingOutOfFrame() {
        if (robotPositionX + robotSpeedX <= 60) {
            robotPositionX = 61;
        }
    }

    public void moveRight() {
        if (!ducked)
            robotSpeedX = WALK_SPEED;
    }

    public void moveLeft() {
        if (!ducked)
            robotSpeedX = -WALK_SPEED;
    }

    public void stopRight() {
        setMovingRight(false);
        updateMovement();
    }

    public void stopLeft() {
        setMovingLeft(false);
        updateMovement();
    }

    private void updateMovement() {
        if (isMovingRight() == false && isMovingLeft() == false)
            robotSpeedX = 0;
        if (isMovingRight() == false && isMovingLeft())
            moveLeft();
        if (isMovingRight() && isMovingLeft())
            moveRight();
    }

    public void jump() {
        if (jumped == false) {
            robotSpeedY = JUMP_SPEED;
            jumped = true;
        }
    }

    public void shoot() {
        Projectile p = new Projectile(robotPositionX + 50, robotPositionY - 25);
        projectiles.add(p);
    }

    public int getCenterX() {
        return robotPositionX;
    }

    public int getCenterY() {
        return robotPositionY;
    }

    public int getSpeedX() {
        return robotSpeedX;
    }

    public int getSpeedY() {
        return robotSpeedY;
    }

    public ArrayList<Projectile> getProjectiles() {
        return projectiles;
    }

    public boolean hasDucked() {
        return ducked;
    }

    public boolean hasJumped() {
        return jumped;
    }

    public boolean isMovingRight() {
        return movingRight;
    }

    public boolean isMovingLeft() {
        return movingLeft;
    }

    public void setMovingRight(boolean movingRight) {
        this.movingRight = movingRight;
    }

    public void setMovingLeft(boolean movingLeft) {
        this.movingLeft = movingLeft;
    }

    public void setCenterX(int centerX) {
        this.robotPositionX = centerX;
    }

    public void setCenterY(int centerY) {
        this.robotPositionY = centerY;
    }

    public void setRobotSpeedX(int robotSpeedX) {
        this.robotSpeedX = robotSpeedX;
    }

    public void setRobotSpeedY(int robotSpeedY) {
        this.robotSpeedY = robotSpeedY;
    }

    public void setDucked(boolean ducked) {
        this.ducked = ducked;
    }

    public void setJumped(boolean jumped) {
        this.jumped = jumped;
    }
}
