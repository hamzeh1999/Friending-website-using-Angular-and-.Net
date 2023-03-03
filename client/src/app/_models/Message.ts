
export interface Message {
    id: Number;
    senderId: Number;
    senderUserName:string;
    senderPhotoUrl:string;
    recipientId:number;
    recipientUserName:string;
    recipientPhotoUrl:string;
    content:string;
    dateRead:Date;
    messageSent:Date ;


}
