import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleCheck, faClose, faExchange } from '@fortawesome/free-solid-svg-icons';
import CreateOrderModal from '../../Components/CreateOrder/index.';
import { fetchOrders, completeOrder } from '../../Services/OrderService';
import './style.css';
import {Order} from "../../Types/Order";


const OrderManagement: React.FC = () => {
    //const [orders, setOrders] = useState<Order[]>([]);
    const [orders, setOrders] = useState<Order[]>([
      {
        id: '1',
        quantity: 10,
        status: 'Ativo',
        expiresOn: new Date(Date.now() + 3600000).toISOString(), // Expira em 1 hora
      },
      {
        id: '2',
        quantity: 5,
        status: 'Concluído',
        expiresOn: new Date(Date.now() - 3600000).toISOString(), // Expirou há 1 hora
      },
      {
        id: '3',
        quantity: 5,
        status: 'Expirado',
        expiresOn: new Date(Date.now() - 3600000).toISOString(), // Expirou há 1 hora
      },
    ]);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  
    const loadOrders = async () => {
      const fetchedOrders = await fetchOrders();
      setOrders(fetchedOrders);
    };

    const handleCompleteOrder = async (orderId: string) => {
      const confirm = window.confirm('Tem certeza que deseja concluir esta encomenda?');
      if (!confirm) return;    
      const success = await completeOrder(orderId);
      if (success) {
        // Atualiza o status da encomenda no estado local
        setOrders((prevOrders) =>
          prevOrders.map((order) =>
            order.id === orderId ? { ...order, status: 'Completed' } : order
          )
        );
        alert('Encomenda concluída com sucesso!');
      } else {
        alert('Erro ao concluir a encomenda.');
      }
    };
  
    useEffect(() => {
      loadOrders();
    }, []);
  
    return (
      <div className="order-management">
        <h2>Gestão de Encomendas</h2>
        <button className="create-order-btn" onClick={() => setIsModalOpen(true)}>Criar Nova Encomenda</button>
        {isModalOpen && <CreateOrderModal onClose={() => setIsModalOpen(false)} onSubmitSuccess={loadOrders} />}
        <table className="order-table">
          <thead>
            <tr>
              <th style={{width: '10%'}}>Identificador</th>
              <th style={{width: '10%'}}>Quantidade</th>
              <th style={{width: '20%'}}>Status</th>
              <th style={{width: '20%'}}>Expiração</th>
              <th style={{width: '10%'}}>Ações</th>
            </tr>
          </thead>
          <tbody>
            {orders.map((order) => (
              <tr key={order.id}>
                <td>{order.id}</td>
                <td>{order.quantity}</td>
                <td>{order.status}</td>
                <td>{new Date(order.expiresOn).toLocaleString()}</td>
                <td>
                {order.status === 'Ativo' && (
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
    );
  };
  
  export default OrderManagement;