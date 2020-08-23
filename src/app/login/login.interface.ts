export interface UserInfo {
    uId?: string;
    userId?: string;
    userName: string;
    email: string;
    userType: string;
    date: Date;
    active: boolean;
    approvalStatus: string;
    approvalBy: string;
    approvalDate: Date;
    imageURL: string;
    password?: string;
    check?: boolean;
}
export class User {
    id: number;
    roleId?: number;
    name: string;
    email: string;
    password: string;
    newPassword: string;
    confirmPassword: string;
    token?: string;
    constructor() {
        this.id = 0;
        this.roleId = null;
        this.name = "";
        this.email = "";
        this.password = "";
        this.token = "";
        this.confirmPassword = "";
        this.newPassword = "";
    }
}
