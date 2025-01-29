import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleCheck, faClose, faExchange } from '@fortawesome/free-solid-svg-icons';
import CreateOrderModal from '../../Components/CreateOrder/index.';
import { fetchOrders, completeOrder } from '../../Services/OrderService';
import { fetchAvStock } from '../../Services/StockService';
import './style.css';
import {Order} from "../../Types/Order";


const OrderManagement: React.FC = () => {
    //const [orders, setOrders] = useState<Order[]>([]);
    const [orders, setOrders] = useState<Order[]>([      
    ]);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [stockAvailable, setStockAvailable] = useState<number>(0);
  
    const loadOrders = async () => {
      let result = await fetchOrders();
        console.log('result', result);
        setOrders(result);
        
    };


    const fetchStock = async () => {
      let result = await fetchAvStock();
      setStockAvailable(result);
    };

    const handleCompleteOrder = async (orderId: string) => {
      const confirm = window.confirm('Tem certeza que deseja concluir esta encomenda?');
      if (!confirm) return;    
      const success = await completeOrder(orderId);
      if (success) {
        // Atualiza o status da encomenda no estado local
        setOrders((prevOrders) =>
          prevOrders.map((order) =>
            order.id === orderId ? { ...order, status: 1, statusStr: 'Completed' } : order 
          )
        );
        alert('Encomenda concluída com sucesso!');
      } else {
        alert('Erro ao concluir a encomenda.');
      }
    };
  
    useEffect(() => {
      loadOrders();
      fetchStock();
    }, []);

    useEffect(() => {
      fetchStock();
    }, [orders]);
  
    return (
      <div className="order-management">
        <h2>Gestão de Encomendas</h2>
        
        <div style={{width: '100%', display: 'flex', justifyContent: 'space-between'}}>
          <h4>Estoque Disponível: {stockAvailable} produtos</h4>
          <div><button className="create-order-btn" style={{alignItems: 'right'}} onClick={() => setIsModalOpen(true)}>Criar Nova Encomenda</button></div>
        </div>
        
        {isModalOpen && <CreateOrderModal onClose={() => setIsModalOpen(false)} onSubmitSuccess={loadOrders} />}
        <div style={{border: '1px solid #ccc', borderRadius:'16px', padding: '20px'}}>
          <table className="order-table">
            <thead>
              <tr>
                <th style={{width: '10%'}}>Identificador</th>
                <th style={{width: '8%'}}>Quantidade</th>
                <th style={{width: '17%'}}>Status</th>
                <th style={{width: '20%'}}>Expiração</th>
                <th style={{width: '15%'}}>Ações</th>
              </tr>
            </thead>
            <tbody>
              {orders.length > 0 && orders.map((order) => (
                <tr key={order.id}>
                  <td>{order.id}</td>
                  <td style={{textAlign: 'center'}}>{order.quantity}</td>
                  <td>{order.statusStr}</td>
                  <td>{new Date(order.expiresOn).toLocaleString()}</td>
                  <td>
                  {order.status === 0 && (
                    <button
                      className="complete-order-btn"
                      onClick={() => handleCompleteOrder(order.id)}
                    >
                      <span style={{fontSize: '0.8rem'}}>Completar</span> <FontAwesomeIcon icon={faExchange} color="green" />
                    </button>
                  )}
                </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    );
  };
  
  export default OrderManagement;